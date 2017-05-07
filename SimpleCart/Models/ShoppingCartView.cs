using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCart.Models
{
    public class ShoppingCartView
    {
        public int ID
        {
            get; internal set;
        }

        public string Code
        {
            get; internal set;
        }

        public string Description
        {
            get; internal set;
        }

        public decimal Price
        {
            get; internal set;
        }

        public int UserId
        {
            get; internal set;
        }

        public int Quantity
        {
            get; internal set;
        }

        public decimal Total
        {
            get; internal set;
        }
    }
}
