import { Address } from './address';
import { Photo } from './photo';

export interface User {
  id: number;
  name: string;
  passwordHash: string;
  passwordSalt: string;
  age: number;
  photoUrl: string;
  knownAs: string;
  lastActive: string;
  gender: string;
  introduction: string;
  lookingFor: string;
  interests: string;
  address: Address;
  photos: Photo[];
}
