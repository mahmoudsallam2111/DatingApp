import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

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
  constructor(private _accountService: AccountService) {}
  ngOnInit(): void {}

  register() {
    this._accountService.register(this.model).subscribe(() => {
      this.cancel();
    });
  }
  cancel() {
    this.cancelRegistration.emit(false);
  }
}
