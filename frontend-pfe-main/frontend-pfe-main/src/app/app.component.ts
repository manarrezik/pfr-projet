import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router'; // ✅ Import this

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet], // ✅ Required for <router-outlet> to work
  templateUrl: './app.component.html',
  // styleUrls: ['./app.component.css'], // Remove or restore based on above
})
export class AppComponent {}
