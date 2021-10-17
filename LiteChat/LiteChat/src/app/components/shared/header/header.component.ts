import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  userData: any;
  constructor() {
    if (localStorage['data']) this.userData = JSON.parse(localStorage['data']);
  }

  ngOnInit(): void {}
}
