import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css',
})
export class ListsComponent implements OnInit {
  members: User[];

  predicate: string = 'like';

  constructor(private _memberService: MembersService) {}
  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    this._memberService.getLikes(this.predicate).subscribe((response) => {
      this.members = response;
    });
  }

  getTitle() {
    this.predicate === 'like' ? 'Member I Like' : 'Member Who liked Me';
  }
}
