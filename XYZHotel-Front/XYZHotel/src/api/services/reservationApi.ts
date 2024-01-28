import axios from "axios";
import { Reservation, ReservationStatus } from "../models";

const api = axios.create({
  baseURL: "https://localhost:7057",
});

export const getReservations = async (): Promise<Reservation[]> => {
  const response = await api.get<Reservation[]>("/GetReservations");
  return response.data;
};

export const getReservation = async (id: string): Promise<Reservation> => {
  const response = await api.get<Reservation>(`/GetReservation/${id}`);
  return response.data;
};

export const createReservation = async (reservation: Reservation): Promise<Reservation> => {
  console.log("Sending reservation data:", reservation); // Ajoutez cette ligne
  const response = await api.post<Reservation>("/CreateReservation", reservation);
  return response.data;
};

export const updateReservationStatus = async (id: string, newStatus: ReservationStatus): Promise<Reservation> => {
  const response = await api.put<Reservation>(`/UpdateReservationStatus/${id}`, { status: newStatus });
  return response.data;
};

export const deleteReservation = async (id: string): Promise<void> => {
  await api.delete(`/DeleteReservation/${id}`);
};
