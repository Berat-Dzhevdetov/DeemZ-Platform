import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ContentComponent } from './components/content/content.component';
import { HeaderComponent } from './components/shared/header/header.component';
import { FooterComponent } from './components/shared/footer/footer.component';
import { NewQuestionComponent } from './components/new-question/new-question.component';
import { QuestionComponent } from './components/question/question.component';
import { QuestionService } from './core/services/question/question.service';

import { DateTimeFormatterPipe } from './components/shared/pipes/date-time-formatter.pipe';
@NgModule({
  declarations: [
    AppComponent,
    ContentComponent,
    HeaderComponent,
    FooterComponent,
    NewQuestionComponent,
    QuestionComponent,
    DateTimeFormatterPipe,
  ],
  imports: [BrowserModule, HttpClientModule, AppRoutingModule],
  providers: [QuestionService],
  bootstrap: [AppComponent],
})
export class AppModule {}
