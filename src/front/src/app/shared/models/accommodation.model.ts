export interface AccommodationModel {
  id: string;
  title: string;
  description: string;
  urlImage: string;
  price: number;
  assignments: AccommodationAssignmentModel[];
}

export interface AccommodationAssignmentModel {
  userId: string;
  username: string;
  responseStatus: string;
}

export interface MyAccommodationModel {
  id: string;
  title: string;
  description: string;
  urlImage: string;
  price: number;
  responseStatus: string;
}
