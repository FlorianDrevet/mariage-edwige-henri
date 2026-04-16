export interface AccommodationModel {
  id: string;
  title: string;
  description: string;
  urlImage: string;
}

export interface UserAccommodationModel {
  id: string;
  title: string;
  description: string;
  urlImage: string;
  isAccepted: boolean | null;
}
