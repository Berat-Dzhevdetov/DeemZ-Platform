import { Component, OnInit } from '@angular/core';
import { LocalStorage } from 'src/environments/LocalStorage';
@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css'],
})
export class FooterComponent implements OnInit {
  year: number;
  constructor(public LocalStorage: LocalStorage) {
    this.year = new Date().getFullYear();
  }

  ngOnInit(): void {}
}
