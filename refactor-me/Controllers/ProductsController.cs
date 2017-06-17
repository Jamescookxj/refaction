using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;
using System.Collections.Generic;
using System.Web;
using System.Net.Http.Formatting;
using Jil;

namespace refactor_me.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        /*1*/
        [Route]
       // [Authorize(Roles = "Administrators")]
        [HttpGet] /*invoke code example: http://localhost:58123/api/products  */
        public List<Product> Products()
        {
            List < Product > allItems= new List<Product>();
            Products ProductOperator = new Products();
            allItems = ProductOperator.GetAllProducts();
            return allItems;
        }

        /*1-paging interface*/
        [Route("{index}/page/{size}")]
        // [Authorize(Roles = "Administrators")]
        [HttpGet] /*invoke code example: http://localhost:58123/api/products/1/page/1  */
        public List<Product> Products(int index, int size)  //index: page number; size: how many records in one page
        {
            List<Product> allItems = new List<Product>();
            Products ProductOperator = new Products();
            allItems = ProductOperator.GetAPageProducts(index, size);
            return allItems;
        }

        /*2*/
        [Route]
        //[Authorize(Roles = "Administrators")]
        [HttpGet] /*invoke code example: http://localhost:58123/api/products?name=Apple%20iPhone%206S  */
        public List<Product> Products(string name)
        {
            List<Product> PartItems  = new List<Product>();
            Products ProductOperator = new Products();
            PartItems = ProductOperator.GetProductsByName(name);
            return PartItems;
        }

        /*3*/
        [Route("{id}")]
       //[Authorize(Roles = "Administrators")]
        [HttpGet] /*invoke code example: http://localhost:58123/api/products/{8f2e9176-35ee-4f0a-ae55-83023d2db1a3}*/
        public Product GetProduct(Guid id)
        {
            List<Product> PartItems = new List<Product>();
            Products ProductOperator = new Products();
            PartItems = ProductOperator.QueryTableByID(id);
            if (PartItems.Count > 0)
            {
                Product product = PartItems[0]; return product;
            }
            else return null;
            //    throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        /*4*/
        [Route]
        //[Authorize(Roles = "Administrators")]
        [HttpPost]
        public int Create(FormDataCollection form)
        {
            int res=0;
            Product newproduct = new Product(0);
            newproduct.Name = form.Get("Name");
            newproduct.Description = form.Get("Description");
            newproduct.Price = decimal.Parse(form.Get("Price"));
            newproduct.DeliveryPrice = decimal.Parse(form.Get("DeliveryPrice"));
            Products ProductOperator = new Products();
            res=ProductOperator.InsertNewProduct(newproduct);
            return res;
        }

        /*5*/
        [Route("{id}")]
        //[Authorize(Roles = "Administrators")]
        [HttpPut]
        public int Update(Guid id, dynamic product)
        {
            int res = 0;
            Product CurrentProduct = new Product(0);
            CurrentProduct.Id = id;
            CurrentProduct.Name= product.Name.ToString();
            CurrentProduct.Description = product.Description.ToString();
            CurrentProduct.Price = decimal.Parse(product.Price.ToString());
            CurrentProduct.DeliveryPrice = decimal.Parse(product.DeliveryPrice.ToString());
            Products ProductOperator = new Products();
            res = ProductOperator.UpdateProduct(CurrentProduct);
            return res;
        }

        /*6*/
        [Route("{id}")]
        //[Authorize(Roles = "Administrators")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            Product product = new Product(id);
            Products ProductOperator = new Products();
            ProductOperator.DeteteAProduct(id);
        }

        /*7*/
        [Route("{productId}/options")]
        //[Authorize(Roles = "Administrators")]
        [HttpGet]
        public List<ProductOption> GetOptions(Guid productId)
        {
            Products ProductOperator = new Products();
            return  ProductOperator.ReturnProductOptions(productId);
        }

        /*8*/
        [Route("{productId}/options/{id}")]
        //[Authorize(Roles = "Administrators")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            ProductOption option = new ProductOption();
            Products ProductOperator = new Products();
            option= ProductOperator.ReturnProductOptionById(productId,id);
            return option;
        }

        /*9*/
        [Route("{productId}/options")]
        //[Authorize(Roles = "Administrators")]
        [HttpPost]
        public int CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            Products ProductOperator = new Products();
            int rec = ProductOperator.InsertNewProductOption(option);
            return rec;
        }

        /*10*/
        [Route("{productId}/options/{id}")]
        //[Authorize(Roles = "Administrators")]
        [HttpPut]
        public int UpdateOption(Guid id, ProductOption option)
        {
            Products ProductOperator = new Products();
            int rec = ProductOperator.UpdateProductOption(option);
            return rec;
        }

        /*11*/
        [Route("{productId}/options/{id}")]
        //[Authorize(Roles = "Administrators")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            Product product = new Product(id);
            Products ProductOperator = new Products();
            ProductOperator.DeteteAProductOption(id);
        }


    }
}
