import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;

  @Input({ required: true }) users: any;
  @Output() cancelRegistration = new EventEmitter();

  maxDate: Date = new Date();
  validationError: string[]; //
  constructor(
    private _accountService: AccountService,
    private _toastr: ToastrService,
    private _router: Router,
    private fb: FormBuilder
  ) {}
  ngOnInit(): void {
    this.initializeForm();
    this.calculateMaxDateForDatePicker();
  }
  calculateMaxDateForDatePicker() {
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18); // regiser only > 18 years old
  }
  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      name: ['', Validators.required],
      knowAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      address: this.fb.group({
        city: ['', Validators.required],
        country: ['', Validators.required],
      }),
      password: [
        '',
        [Validators.required, Validators.minLength(4), Validators.maxLength(8)],
      ],
      confirmPassword: ['', [Validators.required, this.matchValue('password')]],
    });

    this.registerForm.controls['password'].valueChanges.subscribe(() => {
      this.registerForm.controls['confirmPassword'].updateValueAndValidity();
    });
  }

  // custom validtor function this act as an extension method for validators fu
  matchValue(valueToCompare: string): ValidatorFn {
    return (control: AbstractControl) => {
      if (!control.parent) {
        return null; // If there is no parent, validation is not applicable
      }

      const valueToMatch = control.parent.get(valueToCompare);

      if (!valueToMatch) {
        return null; // If the control to compare with is not found, validation is not applicable
      }

      return control.value === valueToMatch.value
        ? null
        : { notMatching: true };
    };
  }
  register() {
    const dob = this.getDateOnly(
      this.registerForm.controls['dateOfBirth'].value
    );

    const values = { ...this.registerForm.value, dateOfBirth: dob };

    this._accountService.register(values).subscribe({
      next: () => {
        this._router.navigateByUrl('/members');
      },
      error: (error) => {
        this.validationError = error;
      },
    });
  }

  private getDateOnly(date: string) {
    let dateOfBirth = new Date(date);
    return new Date(
      dateOfBirth.setMinutes(
        dateOfBirth.getMinutes() - dateOfBirth.getTimezoneOffset()
      )
    )
      .toISOString()
      .slice(0, 10);
  }
  cancel() {
    this.cancelRegistration.emit(false);
  }
}
