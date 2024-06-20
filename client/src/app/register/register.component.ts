import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  model: { username: string; password: string } = {
    username: '',
    password: '',
  };

  @Input({ required: true }) users: any;
  @Output() cancelRegistration = new EventEmitter();
  constructor(
    private _accountService: AccountService,
    private _toastr: ToastrService
  ) {}
  ngOnInit(): void {}

  register() {
    this._accountService.register(this.model).subscribe({
      next: () => this.cancel(),
      error: (error) => this._toastr.error(error.error),
    });
  }
  cancel() {
    this.cancelRegistration.emit(false);
  }
}
