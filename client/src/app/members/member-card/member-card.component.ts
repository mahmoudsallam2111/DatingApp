import { Component, Input, ViewEncapsulation } from '@angular/core';
import { User } from '../../_models/user';
import { MembersService } from '../../_services/members.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css',
})
export class MemberCardComponent {
  @Input({
    required: true,
  })
  member: User;
  constructor(
    private _memberService: MembersService,
    private _toastre: ToastrService
  ) {}

  addLike(member: User) {
    this._memberService.addLike(member.name).subscribe({
      next: (_) => {
        this._toastre.success('you have liked ' + member.knownAs);
      },
      error: (error) => {
        this._toastre.error(error.error.detail);
      },
    });
  }
}
