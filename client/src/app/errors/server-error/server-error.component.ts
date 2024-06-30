import { Component, ElementRef, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.css',
})
export class ServerErrorComponent implements OnInit {
  error: any;

  constructor(private el: ElementRef, private _route: Router) {
    const navigation = this._route.getCurrentNavigation();
    this.error = navigation?.extras?.state ? ['error'] : null;
  }

  ngOnInit(): void {}

  @HostListener('document:mousemove', ['$event'])
  onMouseMove(event: MouseEvent): void {
    const eyes = this.el.nativeElement.querySelectorAll('.eye');
    eyes.forEach((eye: HTMLElement) => {
      const x = eye.offsetLeft + eye.offsetWidth / 2;
      const y = eye.offsetTop + eye.offsetHeight / 2;
      const rad = Math.atan2(event.pageX - x, event.pageY - y);
      const rot = rad * (180 / Math.PI) * -1 + 180;
      eye.style.transform = `rotate(${rot}deg)`;
    });
  }
}
