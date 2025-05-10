import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'; // ✅ For ngModel two-way binding
import { HttpClientModule } from '@angular/common/http'; // ✅ For HTTP requests
// import { AppRoutingModule } from './app-routing.module'; // ✅ For page navigation
import { AppComponent } from './app.component'; // Your root app component


@NgModule({
  declarations: [
    AppComponent,
    // HeaderComponent  // ← très important !
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    // AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
