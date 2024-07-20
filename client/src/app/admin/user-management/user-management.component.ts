import { Component, ModelOptions, OnInit } from '@angular/core';
import { AdminService } from '../../_services/admin.service';
import { AuthUser } from '../../_models/authUser';
import { RolesModalComponent } from '../../modals/roles-modal/roles-modal.component';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { initialState } from 'ngx-bootstrap/timepicker/reducer/timepicker.reducer';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css',
})
export class UserManagementComponent implements OnInit {
  users: AuthUser[];
  bsModalRef: BsModalRef<RolesModalComponent> =
    new BsModalRef<RolesModalComponent>();

  avaliableRoles = ['Admin', 'Moderator', 'Member'];
  constructor(
    private _adminService: AdminService,
    private _modalService: BsModalService,
    private _toastre: ToastrService
  ) {}
  ngOnInit(): void {
    this.loadUsersWithRoles();
  }

  loadUsersWithRoles() {
    this._adminService.getUsersWithRoles().subscribe((users) => {
      this.users = users;
    });
  }

  openRolesModal(user: AuthUser) {
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        username: user.userName,
        avaliableRoles: this.avaliableRoles,
        selectedRoles: [...user.roles],
      },
    };
    this.bsModalRef = this._modalService.show(RolesModalComponent, config);

    this.bsModalRef.onHide.subscribe(() => {
      const seletedRoles = this.bsModalRef.content.selectedRoles;
      if (!this.arrayEqual(seletedRoles, user.roles)) {
        this._adminService
          .updateUserRoles(user.userName, seletedRoles.toString())
          .subscribe({
            next: (data) => {
              user.roles = data;
              this._toastre.success('Role Updated Successfully');
            },
            error: () => {
              this._toastre.error(
                'some thing went wrong while Modifing The role'
              );
            },
          });
      }
    });
  }

  private arrayEqual(arr1: any[], arr2: any[]) {
    return JSON.stringify(arr1.sort()) === JSON.stringify(arr2.sort());
  }
}
