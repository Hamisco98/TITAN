using Microsoft.AspNetCore.Mvc;
using Titan.Areas.Warehouse.APIs.Interfaces;
using Titan.Areas.Warehouse.Models;

namespace Titan.Areas.Warehouse.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartInterface shoppingCartInterface;
        private readonly ICartItemInterface cartItemInterface;
        private readonly IProductsInterfaces productsInterfaces;

        public ShoppingCartController(IShoppingCartInterface shoppingCartInterface, ICartItemInterface cartItemInterface, IProductsInterfaces productsInterfaces)
        {
            this.shoppingCartInterface = shoppingCartInterface;
            this.cartItemInterface = cartItemInterface;
            this.productsInterfaces = productsInterfaces;
        }


        [HttpGet]
        public async Task<IActionResult> GET()
        {
            try
            {
                return Ok(await shoppingCartInterface.GETALL());
            }
            catch (Exception)
            {
                return BadRequest(StatusCode(StatusCodes.Status500InternalServerError, "Error!. Can't Get Data From Database!"));
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GETBYID(Guid id)
        {
            try
            {
                var shoppingCart = await shoppingCartInterface.GETBYID(id);
                if (shoppingCart == null) return NotFound();

                var items = await cartItemInterface.GETALL();
                items = items.Where(x => x.ShoppingCartID == id).ToList();

                shoppingCart.CartItem = items.ToList();

                return Ok(shoppingCart);
            }
            catch (Exception)
            {
                return BadRequest(StatusCode(StatusCodes.Status500InternalServerError, "Error!. Can't Get Data From Database!"));
            }
        }


        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> CreatNewShoopingCart()
        {
            try
            {
                ShoppingCart shoppingCart = new ShoppingCart(){};
                var _shoppingCart = await shoppingCartInterface.POST(shoppingCart);
                return _shoppingCart;
            }
            catch (Exception)
            {
                return BadRequest(StatusCode(StatusCodes.Status500InternalServerError, "Couldn't Create The Shopping Cart!"));
            }
        }


        [HttpPut]
        public async Task<ActionResult<ShoppingCart>> PUT(CartItemViewModel cartItemViewModel)
        {
            try
            {
                //If Quantity == 1 Means new item entred by button - else means update by input filed
                if(cartItemViewModel.quantity == 1)
                {
                    if (cartItemViewModel.barcode == null) return BadRequest("Can't Get The cartItem Header Data!");


                    //Check Shopping Card
                    var shoppingCart = await shoppingCartInterface.GETBYID(cartItemViewModel.shoppingCartID);
                    if(shoppingCart == null) return NotFound("Shopping Cart ID Mismatch");
                    

                    ShoppingCart updateShoppingCart = new ShoppingCart();

                    var product = new Products();

                    if(cartItemViewModel.barcode != "empty")
                    {
                        //Get The Product By Barcode
                        product = await productsInterfaces.GETBARCODE(cartItemViewModel.barcode);
                        if (product == null) return BadRequest(StatusCode(StatusCodes.Status404NotFound, "Barcode Not In The System!"));
                    }
                    if(cartItemViewModel.productID != Guid.Empty)
                    {
                        product = await productsInterfaces.GETBYID(cartItemViewModel.productID);
                        if (product == null) return BadRequest(StatusCode(StatusCodes.Status404NotFound, "Product ID Not In The System!"));
                    }
                    if (product.ID == Guid.Empty) return BadRequest("Somwthing Wrong With This Product!");



                    CartItem cartItem = new CartItem()
                    {
                        ProductBarcode = product.Barcode,
                        ProductID = product.ID,
                        ProductName = product.Name,
                        Cost = product.Cost,
                        Price = product.Price,
                        Quantity = 1,
                        SubTotalCost = product.Cost,
                        SubTotalPrice = product.Price,
                        Date = DateTime.Now,
                        ShoppingCartID = cartItemViewModel.shoppingCartID

                    };

                    //Get All Items With the same ID and ShoppingID
                    var item = await cartItemInterface.GETALL();
                    item = item.Where(c => c.ProductID == cartItem.ProductID).Where(c => c.ShoppingCartID == cartItem.ShoppingCartID).ToList();
                    if (item.Any())
                    {
                        if (item.Count() == 1)
                        {
                            //Update the one in the database
                            CartItem updateItem = item.ToList()[0];

                            cartItem.ID = updateItem.ID;
                            cartItem.ProductBarcode = updateItem.ProductBarcode;
                            cartItem.ProductName = updateItem.ProductName;
                            cartItem.ProductID = updateItem.ProductID;
                            cartItem.ShoppingCartID = updateItem.ShoppingCartID;
                            cartItem.Quantity += updateItem.Quantity;
                            cartItem.Cost = product.Cost;
                            cartItem.Price = product.Price;
                            cartItem.SubTotalCost = cartItem.Quantity * product.Cost;
                            cartItem.SubTotalPrice = cartItem.Quantity * product.Price;
                            cartItem.Date = DateTime.Now;

                        
                            //Update and Get All With The Same Shopping ID
                            await cartItemInterface.PUT(cartItem);
                            var allCartItems = await cartItemInterface.GETALL();
                            allCartItems = allCartItems.Where(c => c.ShoppingCartID == cartItem.ShoppingCartID).ToList();


                            //Update Shopping Cart
                            updateShoppingCart.ID = shoppingCart.ID;
                            updateShoppingCart.Date = DateTime.Now;
                            updateShoppingCart.TotalCosts = shoppingCart.TotalCosts + cartItem.Cost;
                            updateShoppingCart.TotalPrices = shoppingCart.TotalPrices + cartItem.Price;
                            updateShoppingCart.CartItem = allCartItems.ToList();

                            await shoppingCartInterface.PUT(updateShoppingCart);
                            return Ok(updateShoppingCart);
                        }
                        else
                        {
                            return BadRequest("You Pused It. Whyyyyyyyyyyyyyyyyyyyyyyyyy");
                        }
                    }
                    else
                    {
                        //Update and Get All With The Same Shopping ID
                        await cartItemInterface.PUT(cartItem);
                        var allCartItems = await cartItemInterface.GETALL();
                        allCartItems = allCartItems.Where(c => c.ShoppingCartID == cartItem.ShoppingCartID).ToList();


                        //Update Shopping Cart
                        updateShoppingCart.ID = shoppingCart.ID;
                        updateShoppingCart.Date = DateTime.Now;
                        updateShoppingCart.TotalCosts = shoppingCart.TotalCosts + cartItem.Cost;
                        updateShoppingCart.TotalPrices = shoppingCart.TotalPrices + cartItem.Price;
                        updateShoppingCart.CartItem = allCartItems.ToList();

                        await shoppingCartInterface.PUT(updateShoppingCart);
                        return Ok(updateShoppingCart);
                    }
                }
                else
                {
                    //Should Update The Item
                    return null ;
                }
            }
            catch (Exception)
            {
                return BadRequest(StatusCode(StatusCodes.Status500InternalServerError, "Error!. Can't PUT Data To Database!"));
            }
        }


        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            try
            {
                var item = await cartItemInterface.GETBYID(id);
                if (item == null) return NotFound("Item Not Found, Please Contact The Admin For That Problem!");
                
                await cartItemInterface.DELETE(id);

                var shoppingCart = await shoppingCartInterface.GETBYID(item.ShoppingCartID);
                if (shoppingCart == null) return NotFound("Shopping Cart ID Mismatch");

                var cartItems = await cartItemInterface.GETALL();
                cartItems = cartItems.Where(x => x.ShoppingCartID == shoppingCart.ID).ToList();

                ShoppingCart updateShoppingCart = new ShoppingCart()
                {
                    ID = shoppingCart.ID,
                    Date = shoppingCart.Date,
                    TotalCosts = shoppingCart.TotalCosts - item.SubTotalCost,
                    TotalPrices = shoppingCart.TotalPrices - item.SubTotalPrice,
                    IsActive = shoppingCart.IsActive,
                    IsSuccessful = shoppingCart.IsSuccessful,
                    CartItem = cartItems.ToList()
                };
                await shoppingCartInterface.PUT(updateShoppingCart);
                return Ok(updateShoppingCart);
            }
            catch (Exception)
            {

                return BadRequest(StatusCode(StatusCodes.Status500InternalServerError, "Couldn't Delete The Item!"));
            }
        }


        [HttpDelete("{id:Guid}/{_for:int}")]
        public async Task<IActionResult> DeleteCart(Guid id, int _for)
        {
            //Get And Delete The Cart
            var shoppingCart = await shoppingCartInterface.GETBYID(id);
            if(shoppingCart == null) return NotFound("Error, Shopping Cart Not Found!!");

            await shoppingCartInterface.DELETE(id);

            var cartItems = await cartItemInterface.GETALL();
            cartItems = cartItems.Where(x => x.ShoppingCartID == id).ToList();

            foreach(var item in cartItems)
            {
                await cartItemInterface.DELETE(item.ID);
            }
            //Get All Item With The Same Cart ID And Delete Them All

            return Ok();
        }


        [HttpPost("{id:Guid}/{val:bool}")]
        public async Task<ActionResult> Checkout(Guid id,bool val)
        {
            //Get Shopping Cart
            if(val)
            {
                var shoppingCart = await shoppingCartInterface.GETBYID(id);
                if (shoppingCart == null) return NotFound("Error, Shopping Cart Not Found!!");

                var items = await cartItemInterface.GETALL();
                items = items.Where(x => x.ShoppingCartID == id).ToList();
                if (items.Any())
                {
                    ShoppingCart _shoppingCart = new ShoppingCart()
                    {
                        ID = id,
                        TotalCosts = shoppingCart.TotalCosts,
                        TotalPrices = shoppingCart.TotalPrices,
                        Date = DateTime.Now,
                        IsActive = false,
                        IsSuccessful = true,
                        CartItem = shoppingCart.CartItem
                    };
                    await shoppingCartInterface.PUT(_shoppingCart);
                    return Ok(_shoppingCart);
                }
                else return BadRequest("There Are No Items In The Cart!");

            }
            else return BadRequest(StatusCode(StatusCodes.Status203NonAuthoritative,"You Are using the wrong resource for this operation!"));
        }
    }
}

/* 
 [HttpPut]
        public async Task<ActionResult<ShoppingCart>> PUT(CartItemViewModel cartItemViewModel)
        {
            try
            {
                if (cartItemViewModel.barcode == null) return BadRequest("Can't Get The cartItem Header Data!");
                

                //Check Shopping Card
                var shoppingCart = await shoppingCartInterface.GETBYID(cartItemViewModel.shoppingCartID);
                if(shoppingCart == null) return NotFound("Shopping Cart ID Mismatch");
                ShoppingCart updateShoppingCart = new ShoppingCart();


                //Get The Product
                var product = await productsInterfaces.GETBARCODE(cartItemViewModel.barcode);
                if (product == null) return BadRequest(StatusCode(StatusCodes.Status404NotFound, "Barcode Not In The System!"));

                CartItem cartItem = new CartItem()
                {
                    ProductBarcode = product.Barcode,
                    ProductID = product.ID,
                    ProductName = product.Name,
                    Cost = product.Cost,
                    Price = product.Price,
                    Quantity = 1,
                    SubTotalCost = product.Cost,
                    SubTotalPrice = product.Price,
                    Date = DateTime.Now,
                    ShoppingCartID = cartItemViewModel.shoppingCartID

                };

                //Get All Items With the same ID and ShoppingID
                var item = await cartItemInterface.GETALL();
                item = item.Where(c => c.ProductID == cartItem.ProductID).Where(c => c.ShoppingCartID == cartItem.ShoppingCartID).ToList();
                if (item.Any())
                {
                    if (item.Count() == 1)
                    {
                        //Update the one in the database
                        CartItem updateItem = item.ToList()[0];

                        cartItem.ID = updateItem.ID;
                        cartItem.ProductBarcode = updateItem.ProductBarcode;
                        cartItem.ProductName = updateItem.ProductName;
                        cartItem.ProductID = updateItem.ProductID;
                        cartItem.ShoppingCartID = updateItem.ShoppingCartID;
                        cartItem.Quantity += updateItem.Quantity;
                        cartItem.Cost = product.Cost;
                        cartItem.Price = product.Price;
                        cartItem.SubTotalCost = cartItem.Quantity * product.Cost;
                        cartItem.SubTotalPrice = cartItem.Quantity * product.Price;
                        cartItem.Date = DateTime.Now;

                        
                        //Update and Get All With The Same Shopping ID
                        await cartItemInterface.PUT(cartItem);
                        var allCartItems = await cartItemInterface.GETALL();
                        allCartItems = allCartItems.Where(c => c.ShoppingCartID == cartItem.ShoppingCartID).ToList();


                        //Update Shopping Cart
                        updateShoppingCart.ID = shoppingCart.ID;
                        updateShoppingCart.Date = DateTime.Now;
                        updateShoppingCart.TotalCosts = shoppingCart.TotalCosts + cartItem.Cost;
                        updateShoppingCart.TotalPrices = shoppingCart.TotalPrices + cartItem.Price;
                        updateShoppingCart.CartItem = allCartItems.ToList();

                        await shoppingCartInterface.PUT(updateShoppingCart);
                        return Ok(updateShoppingCart);
                    }
                    else
                    {
                        return BadRequest("You Pused It. Whyyyyyyyyyyyyyyyyyyyyyyyyy");
                    }
                }
                else
                {
                    //Update and Get All With The Same Shopping ID
                    await cartItemInterface.PUT(cartItem);
                    var allCartItems = await cartItemInterface.GETALL();
                    allCartItems = allCartItems.Where(c => c.ShoppingCartID == cartItem.ShoppingCartID).ToList();


                    //Update Shopping Cart
                    updateShoppingCart.ID = shoppingCart.ID;
                    updateShoppingCart.Date = DateTime.Now;
                    updateShoppingCart.TotalCosts = shoppingCart.TotalCosts + cartItem.Cost;
                    updateShoppingCart.TotalPrices = shoppingCart.TotalPrices + cartItem.Price;
                    updateShoppingCart.CartItem = allCartItems.ToList();

                    await shoppingCartInterface.PUT(updateShoppingCart);
                    return Ok(updateShoppingCart);
                }
            }
            catch (Exception)
            {
                return BadRequest(StatusCode(StatusCodes.Status500InternalServerError, "Error!. Can't PUT Data To Database!"));
            }
        }
 
 */