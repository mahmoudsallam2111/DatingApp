import { AuthUser } from './authUser';

export class UserParams {
  page?: number = 1;
  itemPerPage?: number = 5;
  gender?: string;
  minAge?: number = 18;
  maxAge?: number = 99;
  constructor(user: AuthUser) {
    this.gender = user.gender === 'male' ? 'male' : 'female';
  }
}
