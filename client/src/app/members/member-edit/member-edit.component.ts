import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { User } from '../../_models/user';
import { AuthUser } from '../../_models/authUser';
import { AccountService } from '../../_services/account.service';
import { MembersService } from '../../_services/members.service';
import { take } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';
import { UserUpdateDto } from '../../_models/userUpdateDto';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css',
})
export class MemberEditComponent implements OnInit {
  member: User;
  user: AuthUser;
  @ViewChild('editForm') editForm: NgForm;
  // to prevent notify user when navigating outside the application
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }

  userUpdateDto: UserUpdateDto;
  constructor(
    private _accountService: AccountService,
    private _memberservice: MembersService,
    private _toastre: ToastrService,
    private _router: ActivatedRoute,
    private _routerService: Router
  ) {}
  ngOnInit(): void {
    this._accountService.currentUser$.pipe(take(1)).subscribe((data) => {
      this.user = data;
    });
    this.loadProfile();
  }

  loadProfile() {
    if (!this.user) return;
    this._memberservice.getMember(this.user.name).subscribe((response) => {
      this.member = response;
    });
  }

  updateMember() {
    const id = Number(this._router.snapshot.paramMap.get('id'));
    const userUpdateDto = { ...this.editForm.value, id } as UserUpdateDto;

    this._memberservice.updateMember(userUpdateDto).subscribe(() => {
      this._toastre.success('profile updated successfully');
      this.editForm?.reset(this.member);
      this._routerService.navigateByUrl('/members');
    });
  }
}
