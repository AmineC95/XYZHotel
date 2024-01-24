import axios from "axios";
import { Reservation } from "../models";

const api = axios.create({
  baseURL: "https://localhost:7057",
});

export const getReservations = async (): Promise<Reservation[]> => {
  const response = await api.get<Reservation[]>("/GetReservations");
  return response.data;
};

export const createReservation = async (
  newReservation: Reservation,
): Promise<Reservation> => {
  const response = await api.post<Reservation>("/CreateReservation", newReservation);
  return response.data;
};

export const updateReservation = async (
  id: string,
  updatedReservation: Reservation,
): Promise<void> => {
  await api.put(`/UpdateReservation/${id}`, updatedReservation);
};

export const deleteReservation = async (id: string): Promise<void> => {
  await api.delete(`/DeleteReservation/${id}`);
};
