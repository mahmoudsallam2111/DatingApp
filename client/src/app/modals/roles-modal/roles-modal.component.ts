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
  title: string = '';
  list: any = {};
  closeBtnName: string = '';
  constructor(public bsModalRef: BsModalRef) {} // private _adminService: AdminService,
  ngOnInit(): void {}
}
