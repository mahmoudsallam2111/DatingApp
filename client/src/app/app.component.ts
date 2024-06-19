import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  title = 'client';
  user: any;
  constructor(private _http: HttpClient) {}
  ngOnInit(): void {
    this._http
      .get('https://localhost:5001/api/User/2')
      .subscribe((response) => {
        this.user = response;
        console.log(this.user);
      });
  }
}
