import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JoinChatComponent } from './join-chat.component';

describe('JoinChatComponent', () => {
  let component: JoinChatComponent;
  let fixture: ComponentFixture<JoinChatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JoinChatComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(JoinChatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
