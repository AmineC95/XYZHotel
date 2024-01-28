import axios from "axios";
import { Customer, LoginRequest,Room } from "../models";

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

export const createWallet = async (id: string): Promise<void> => {
  await api.post(`/CreateWallet/${id}`);
};

export const getWallet = async (id: string): Promise<any> => {
  try {
    const response = await api.get(`/GetWallet/${id}`);
    return response.data;
  } catch (error:any) {
    if (error.response && error.response.status === 404) {
      return null;
    } else {
      throw error;
    }
  }
};

export const creditWallet = async (id: string, amount: number, currency: string): Promise<any> => {
  const response = await api.post(`/CreditWallet/${id}`, { Amount: amount, Currency: currency });
  return response.data;
};

export const debitWallet = async (id: string, amount: number, currency: string): Promise<any> => {
  const response = await api.post(`/DebitWallet/${id}`, { Amount: amount, Currency: currency });
  return response.data;
};


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

//Room
export const getRooms = async (): Promise<Room[]> => {
  const response = await api.get<Room[]>("/api/room");
  return response.data;
};

export const createReservation = async (reservation: Reservation): Promise<Reservation> => {
  console.log("Sending reservation data:", reservation); // Ajoutez cette ligne
  const response = await api.post<Reservation>("/CreateReservation", reservation);
  return response.data;
};

export default api;
