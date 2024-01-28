/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface Balance {
  /** @format double */
  Amount?: number | null;
  Currency?: Currency;
}

export enum Currency {
  EUR = "EUR",
  USD = "USD",
  GBP = "GBP",
  JPY = "JPY",
  CHF = "CHF",
}

export interface Customer {
  /** @format uuid */
  Id?: string | null;
  FullName?: string | null;
  Email?: Email;
  PhoneNumber?: PhonesNumber;
  PasswordHash?: string | null;
}

export interface Email {
  /** @format email */
  Value?: string | null;
}

export interface LoginRequest {
  Email?: string | null;
  Password?: string | null;
}

export interface PhonesNumber {
  /** @format tel */
  Number?: string | null;
}

export interface Reservation {
  /** @format int32 */
  Id?: number | null;
  Customer?: Customer;
  Room?: Room;
  /** @format date-time */
  CheckInDate?: string | null;
  /** @format date-time */
  CheckOutDate?: string | null;
  /** @format int32 */
  NumberOfNights?: number;
  Status?: ReservationStatus;
}

export enum ReservationStatus {
  Pending = "Pending",
  Confirmed = "Confirmed",
  Cancelled = "Cancelled",
}

export interface Room {
  /** @format int32 */
  Id?: number;
  Type?: RoomType;
  PricePerNight?: Balance;
}

export enum RoomType {
  Standard = "Standard",
  Superior = "Superior",
  Suite = "Suite",
}
