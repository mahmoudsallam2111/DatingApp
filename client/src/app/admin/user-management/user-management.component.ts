import { Component, ModelOptions, OnInit } from '@angular/core';
import { AdminService } from '../../_services/admin.service';
import { AuthUser } from '../../_models/authUser';
import { RolesModalComponent } from '../../modals/roles-modal/roles-modal.component';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css',
})
export class UserManagementComponent implements OnInit {
  users: AuthUser[];
  bsModalRef: BsModalRef<RolesModalComponent> =
    new BsModalRef<RolesModalComponent>();
  constructor(
    private _adminService: AdminService,
    private _modalService: BsModalService
  ) {}
  ngOnInit(): void {
    this.loadUsersWithRoles();
  }

  loadUsersWithRoles() {
    this._adminService.getUsersWithRoles().subscribe((users) => {
      this.users = users;
    });
  }

  openRolesModal() {
    const initialState: ModalOptions = {
      initialState: {
        list: [
          'Open a modal with component',
          'Pass your data',
          'Do something else',
          '...',
        ],
        title: 'Modal with component',
      },
    };
    this.bsModalRef = this._modalService.show(
      RolesModalComponent,
      initialState
    );

    this.bsModalRef.content.closeBtnName = 'Close';
  }
}
