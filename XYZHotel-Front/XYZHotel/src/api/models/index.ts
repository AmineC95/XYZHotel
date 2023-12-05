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
  amount?: number | null;
  currency?: Currency;
}

/** @format int32 */
export enum Currency {
  Value0 = 0,
  Value1 = 1,
  Value2 = 2,
  Value3 = 3,
  Value4 = 4,
}

export interface Customer {
  /** @format uuid */
  id?: string | null;
  fullName?: string | null;
  email?: Email;
  phoneNumber?: PhonesNumber;
  passwordHash?: string | null;
}

export interface Email {
  /** @format email */
  value?: string | null;
}

export interface LoginRequest {
  email?: string | null;
  password?: string | null;
}

export interface PhonesNumber {
  /** @format tel */
  number?: string | null;
}

export interface Reservation {
  /** @format uuid */
  id?: string | null;
  customer?: Customer;
  room?: Room;
  /** @format date-time */
  checkInDate?: string | null;
  /** @format date-time */
  checkOutDate?: string | null;
  /** @format int32 */
  numberOfNights?: number;
  status?: ReservationStatus;
}

/** @format int32 */
export enum ReservationStatus {
  Value0 = 0,
  Value1 = 1,
  Value2 = 2,
}

export interface Room {
  /** @format uuid */
  id?: string | null;
  type?: RoomType;
  pricePerNight?: Balance;
}

/** @format int32 */
export enum RoomType {
  Value0 = 0,
  Value1 = 1,
  Value2 = 2,
}

export interface WeatherForecast {
  /** @format date */
  date?: string;
  /** @format int32 */
  temperatureC?: number;
  /** @format int32 */
  temperatureF?: number;
  summary?: string | null;
}
