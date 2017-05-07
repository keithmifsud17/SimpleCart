import React, { Component } from 'react';
import './cart.css';

class ShoppingCart extends Component {
	constructor(props){
		super(props);
		
		this.state= {
			carts: []
		}
	}
	
	componentDidMount() {
		this.RefreshCart();
	}
	
	RefreshCart = () => {
		var that = this;

		var myInit = { 
			method: 'GET'
		};

		var myRequest = new Request('http://localhost:61885/api/shoppingcarts', myInit);
		fetch(myRequest).then(function(response) {
			var contentType = response.headers.get("content-type");
			if(response.status === 200 && contentType && contentType.indexOf("application/json") !== -1) {
				return response.json();
			} else {
				console.log("Oops, we haven't got JSON!");
				return null;
			}
		}).then(function(json) {
			that.setState({carts: json});		
		});
	}
	
	DeleteAll = () => {
		var that = this;
		
		var myInit = { 
			method: 'DELETE',
			mode: 'cors'
		};

		var myRequest = new Request('http://localhost:61885/api/shoppingCarts/', myInit);
		fetch(myRequest).then(function(response) {
			if(response.status === 200) {
				alert('All Items were deleted from your cart.');
			} else {
				alert('An error was encountered while trying to complete your request. Please try again.');
			}
		}).then(that.RefreshCart);
	}
	
	render() {
		return (
			<div>
				<h2>Shopping Cart</h2>
				<table className='cartTable'>
					<thead>
						<tr>
							<th>Code</th>
							<th>Description</th>
							<th>Price</th>
							<th>Quantity</th>
							<th>Total</th>
							<th><button onClick={this.DeleteAll}>DELETE ALL</button></th>
						</tr>
					</thead>
					<tbody>
						{this.state.carts.map(c => <Cart key={c.id} cart={c} refreshCart={this.RefreshCart} /> )}
					</tbody>
				</table>
			</div>
		);
	}
}
class Cart extends Component {
	cart = null;
	refreshCart;
	
	constructor(props){
		super(props);
		
		this.cart = props.cart;
		this.refreshCart = props.refreshCart;
	}
	
	DeleteItem = () => {
		var that = this;
		
		var myInit = { 
			method: 'DELETE',
			mode: 'cors'
		};

		var myRequest = new Request('http://localhost:61885/api/shoppingCarts/' + this.cart.id, myInit);
		fetch(myRequest).then(function(response) {
			if(response.status === 200) {
				alert('Item was deleted from your cart.');
			} else {
				alert('An error was encountered while trying to complete your request. Please try again.');
			}
		}).then(that.refreshCart);
	}
	
	render() {
		return (
			<tr>
				<td>{this.cart.code}</td>
				<td>{this.cart.description}</td>
				<td>&euro;{this.cart.price}</td>
				<td>{this.cart.quantity}</td>
				<td>&euro;{this.cart.total}</td>
				<td><button onClick={this.DeleteItem}>Delete</button></td>
			</tr>
		);
	}
}

export default ShoppingCart;