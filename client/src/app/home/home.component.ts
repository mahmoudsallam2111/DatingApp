import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  registerMode: boolean = false;
  users: any;

  constructor(private _http: HttpClient) {}
  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this._http
      .get('https://localhost:5001/api/User/getAllUers')
      .subscribe((data) => {
        this.users = data;
        console.log(this.users);
      });
  }
  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  onCancelRegisteration(data) {
    this.registerMode = data;
  }
}
