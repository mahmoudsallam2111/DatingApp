import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { MembersService } from '../../_services/members.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css',
})
export class MemberListComponent implements OnInit {
  members$: Observable<User[]>;
  constructor(private _memeberservice: MembersService) {}
  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.members$ = this._memeberservice.getMembers(); // to take the advantage of unsubscribe
  }
}
