import axios from "axios";
import { Customer, LoginRequest } from "../models";

const api = axios.create({
  baseURL: "https://localhost:7057",
});

export const getCustomers = async (): Promise<Customer[]> => {
  const response = await api.get<Customer[]>("/GetCustomer");
  return response.data;
};

export const createCustomer = async (
  newCustomer: Customer,
): Promise<Customer> => {
  const response = await api.post<Customer>("/CreateCustomer", newCustomer);
  return response.data;
};

// export const updateCustomer = async (id: string, updatedCustomer: Customer): Promise<void> => {
//   const response = await api.put<void>(`/UpdateCustomer/${id}`, updatedCustomer);
//   console.log(response.data);
//   return response.data;
// };

export const updateCustomer = async (id: string, updatedCustomer: Customer): Promise<void> => {
  console.log(`Updating customer with ID ${id}`);
  console.log('Updated customer data:', updatedCustomer);

  const response = await api.put<void>(`/UpdateCustomer/${id}`, updatedCustomer, {
    headers: { 'Content-Type': 'application/json' }
  });

  console.log('Response data:', response.data);
  return response.data;
};

export const deleteCustomer = async (id: string): Promise<void> => {
  await api.delete(`/DeleteCustomer/${id}`);
};

export const login = async (loginRequest: LoginRequest): Promise<string> => {
  const response = await api.post<string>("/Login", loginRequest);
  console.log(response.data);
  return response.data;
};


export const getUserInfo = async (): Promise<any> => {
  const response = await api.get<any>("/GetUserInfo", {
    headers: {
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    }
  });
  return response.data;
};

export default api;
