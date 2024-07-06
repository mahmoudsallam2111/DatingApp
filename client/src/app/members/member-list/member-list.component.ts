import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { MembersService } from '../../_services/members.service';
import { Observable, take } from 'rxjs';
import { Pagination } from '../../_models/pagination';
import { UserParams } from '../../_models/userParams';
import { AuthUser } from '../../_models/authUser';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css',
})
export class MemberListComponent implements OnInit {
  // members$: Observable<User[]>;
  members: User[];
  pagination: Pagination | undefined;
  userParams: UserParams;
  user: AuthUser;
  constructor(
    private _memeberservice: MembersService,
    private _accountService: AccountService
  ) {
    this._accountService.currentUser$.pipe(take(1)).subscribe((user) => {
      debugger;
      if (user) {
        this.user = user;
        this.userParams = new UserParams(user);
      }
    });
  }
  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this._memeberservice.getMembers(this.userParams).subscribe((response) => {
      if (response.result && response.pagination) {
        this.members = response.result;
        this.pagination = response.pagination;
      }
    });
  }

  pageChanged(event: any) {
    if (this.userParams.page !== event.page) {
      this.userParams.page = event.page;
      this.loadMembers();
    }
  }
}
