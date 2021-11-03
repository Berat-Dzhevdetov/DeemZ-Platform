import {
  Component,
  OnInit,
  ViewChild,
  EventEmitter,
  Output,
} from '@angular/core';
import { environment } from 'src/environments/environment';
import { ContentComponent } from '../content/content.component';
@Component({
  selector: 'app-popup',
  templateUrl: './popup.component.html',
  styleUrls: ['./popup.component.css'],
})
export class PopupComponent implements OnInit {
  selectedSort!: string;

  @Output() messageEvent = new EventEmitter<string>();

  constructor() {}

  ngOnInit(): void {}

  getPopupState() {
    return environment.userOptions.burgerState;
  }

  closeBurger() {
    environment.userOptions.burgerState = !environment.userOptions.burgerState;
  }

  changeTheme(event: Event) {
    console.log('changeTheme', event);
  }

  sortOptions() {
    if (this.selectedSort != '') {
      environment.userOptions.messageOrderState = this.selectedSort;
    }
    this.messageEvent.emit('HELLO');
    this.closeBurger();
  }
}
