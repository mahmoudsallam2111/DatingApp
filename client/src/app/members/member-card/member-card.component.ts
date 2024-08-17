import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { User } from '../../_models/user';
import { MembersService } from '../../_services/members.service';
import { ToastrService } from 'ngx-toastr';
import { PresenceService } from '../../_services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css',
})
export class MemberCardComponent implements OnInit {
  @Input({
    required: true,
  })
  member: User;
  constructor(
    private _memberService: MembersService,
    private _toastre: ToastrService,
    public _PresenceService: PresenceService
  ) {}
  ngOnInit(): void {}

  addLike(member: User) {
    this._memberService.addLike(member.userName).subscribe({
      next: (_) => {
        this._toastre.success('you have liked ' + member.knownAs);
      },
      error: (error) => {
        this._toastre.error(error.error.detail);
      },
    });
  }
}
