import React, { Component } from 'react';
import Products from './Products';
import ShoppingCart from './Cart';

class Menu extends Component {
	
	constructor(props){
		super(props);
		
		this.state= {
			products: true
		}
	}
	
	ShowProducts = (e) => {
		e.preventDefault();
		this.setState({products: true});
	}
	
	ShowCart = (e) => {
		e.preventDefault();
		this.setState({products: false});
	}
	
	render() {
		return (
			<div>
				<h1>Simple Shopping Cart</h1>
				<a href='#' onClick={this.ShowProducts}>Products</a>
				<a href='#' onClick={this.ShowCart}>My Shopping Cart</a>
				{this.state.products ? <Products /> : <ShoppingCart />}
			</div>
		);
	}
}

export default Menu;