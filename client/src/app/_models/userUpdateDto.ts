import { Address } from './address';

export interface UserUpdateDto {
  id: number;
  introduction: string;
  lookingFor: string;
  interests: string;
  address: Address;
}
