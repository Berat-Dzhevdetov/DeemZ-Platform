import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AngularFireModule } from '@angular/fire';
import { AngularFirestoreModule } from '@angular/fire/firestore';
import { AngularFireStorageModule } from '@angular/fire/storage';
import { AngularFireAuthModule } from '@angular/fire/auth';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ContentComponent } from './components/content/content.component';
import { HeaderComponent } from './components/shared/header/header.component';
import { FooterComponent } from './components/shared/footer/footer.component';
import { NewQuestionComponent } from './components/new-question/new-question.component';
import { NotFoundComponent } from './components/shared/not-found/not-found.component';
import { QuestionComponent } from './components/question/question.component';
import { QuestionService } from './core/services/question/question.service';

import { DateTimeFormatterPipe } from './components/shared/pipes/date-time-formatter.pipe';
import { ShortenTextPipe } from './components/shared/pipes/shorten-text.pipe';

import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { environment } from 'src/environments/environment';
import { PopupComponent } from './components/popup/popup.component';

@NgModule({
  declarations: [
    AppComponent,
    ContentComponent,
    HeaderComponent,
    FooterComponent,
    NewQuestionComponent,
    QuestionComponent,
    DateTimeFormatterPipe,
    ShortenTextPipe,
    NotFoundComponent,
    PopupComponent,
  ],
  imports: [
    BrowserModule,
    AngularFireModule.initializeApp(environment.firebase),
    AngularFirestoreModule,
    AngularFireStorageModule,
    AngularFireAuthModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    MatProgressSpinnerModule,
    FormsModule,
  ],
  providers: [QuestionService],
  bootstrap: [AppComponent],
})
export class AppModule {}
