import React, { Component } from 'react';
import './products.css';

class Products extends Component {
	
	constructor(props){
		super(props);
		
		this.state= {
			products: []
		}
	}
	
	componentDidMount() {
		var that = this;

		var myInit = { 
			method: 'GET'
		};

		var myRequest = new Request('http://localhost:61885/api/products', myInit);
		fetch(myRequest).then(function(response) {
			var contentType = response.headers.get("content-type");
			if(response.status === 200 && contentType && contentType.indexOf("application/json") !== -1) {
				return response.json();
			} else {
				console.log("Oops, we haven't got JSON!");
				return null;
			}
		}).then(function(json) {
			that.setState({products: json});
		});
	}
	
	render() {
		return (
			<div>
				<h2>Product List</h2>
				{this.state.products.map(p => <Product key={p.id} product={p} /> )}
			</div>
		);
	}
}

class Product extends Component {
	product = null;
	
	constructor(props){
		super(props);
		
		this.product = props.product;
		this.state = { quantity : 1 };
	}
	
	AddToCart = () => {
		var that = this;
		var cart = { UserId: 1, ProductId: this.product.id, Quantity: this.state.quantity};
		
		var myInit = { 
			headers: {
				'Accept': 'application/json',
				'Content-Type': 'application/json'
			},
			method: 'POST',
			mode: 'cors',
			body: JSON.stringify(cart)
		};

		var myRequest = new Request('http://localhost:61885/api/shoppingCarts', myInit);
		fetch(myRequest).then(function(response) {
			var contentType = response.headers.get("content-type");
			if(response.status === 201 && contentType && contentType.indexOf("application/json") !== -1) {
				that.setState({quantity: 1});
				alert('Item was added to your cart.');
			} else {
				console.log("Oops, we haven't got JSON!");
				alert('An error was encountered while trying to complete your request. Please try again.');
			}
		});
	}
	
	DecQuantity = () => {
		var q = parseInt(this.state.quantity,10);
		if (q > 1) {
			this.setState({quantity: q - 1});
		}
	}
	
	IncQuantity = () => {
		var q = parseInt(this.state.quantity,10);
		this.setState({quantity: q + 1});
	}
	
	UpdateQuantity = (e) => {
		this.setState({quantity: e.target.value});
	}
	
	render() {
		return (
			<div className='productContainer'>
				<h3 className='productTitle'>{this.product.description}</h3>
				<div>
					<span>{this.product.code}</span>
					<span className='productPrice'><strong>&euro;{this.product.price}</strong></span>
				</div>
				<hr />
				<div>
					<div className='productQuantity'>
						<button onClick={this.DecQuantity}>-</button>
						<input type='number' min='1' value={this.state.quantity} onChange={this.UpdateQuantity} />
						<button onClick={this.IncQuantity}>+</button>
					</div>
					<div className='productAdd'>
						<button onClick={this.AddToCart}>Add to Cart</button>
					</div>
				</div>
			</div>
		);
	}
}

export default Products;