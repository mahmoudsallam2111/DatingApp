export interface AuthUser {
  id: number;
  userName: string;
  token: string;
  photoUrl: string;
  knownAs: string;
  gender: string;
  roles: string[];
}
