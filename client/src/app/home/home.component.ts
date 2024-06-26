import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  registerMode: boolean = false;
  users: any;

  constructor() {}
  ngOnInit(): void {}

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  onCancelRegisteration(data) {
    this.registerMode = data;
  }
}
