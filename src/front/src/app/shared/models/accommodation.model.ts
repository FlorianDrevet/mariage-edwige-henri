export interface AccommodationModel {
  id: string;
  title: string;
  description: string;
  urlImage: string;
  pricePerPersonPerNight: number;
  numberOfNights: number;
  capacity: number;
  remainingCapacity: number;
  bookings: BookingModel[];
}

export interface BookingModel {
  id: string;
  accommodationId: string;
  accommodationTitle: string;
  accommodationImage: string;
  numberOfPersons: number;
  totalAmount: number;
  status: string;
  bookerFirstName: string;
  bookerLastName: string;
  bookerEmail: string;
  createdAt: string;
}

export interface AvailableAccommodationModel {
  id: string;
  title: string;
  description: string;
  urlImage: string;
  pricePerPersonPerNight: number;
  numberOfNights: number;
  capacity: number;
  remainingCapacity: number;
}

export interface MyBookingsModel {
  bookings: BookingModel[];
}

export interface BookAccommodationRequest {
  numberOfPersons: number;
  firstName: string;
  lastName: string;
  email?: string;
}
