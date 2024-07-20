import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../_services/admin.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AuthUser } from '../../_models/authUser';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrl: './roles-modal.component.css',
})
export class RolesModalComponent implements OnInit {
  userName: string = '';
  avaliableRoles: any[] = [];
  selectedRoles: any[] = [];
  closeBtnName: string = '';
  constructor(public bsModalRef: BsModalRef) {}
  ngOnInit(): void {}

  updateChecked(checkedValue: string) {
    const index = this.selectedRoles.indexOf(checkedValue);
    index !== -1
      ? this.selectedRoles.splice(index, 1)
      : this.selectedRoles.push(checkedValue);
  }
}
