export interface Product {
  id: number;
  name: string;
  price: number;
}

export interface CartItem {
  id: number;
  quantity: number;
  product: Product;
}

export interface ShoppingCart {
  id: number;
  userId: number;
  cartItems: CartItem[];
}