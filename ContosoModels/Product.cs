//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//  The MIT License (MIT)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------

namespace Models
{
    /// <summary>
    /// Represents a product.
    /// </summary>
    public class Product : DbObject
    {
        /// <summary>
        /// Gets or sets the product's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the product's color.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the days required to manufacture the product.
        /// </summary>
        public int DaysToManufacture { get; set; }

        /// <summary>
        /// Gets or sets the product's standard cost.
        /// </summary>
        public decimal StandardCost { get; set; }

        /// <summary>
        /// Gets or sets the product's list price.
        /// </summary>
        public decimal ListPrice { get; set; }

        /// <summary>
        /// Gets or sets the product's weight.
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets the product's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Returns the name of the product and the list price.
        /// </summary>
        public override string ToString() => $"{Name} \n{ListPrice}";
    }
}