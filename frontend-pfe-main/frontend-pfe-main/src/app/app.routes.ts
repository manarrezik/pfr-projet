import type { Routes } from "@angular/router";
import { LoginComponent } from "./components/login/login.component";
import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { UserManagementComponent } from './components/user-management/user-management.component';
import { AdminGuard } from "./guards/admin.guard";

export const routes: Routes = [
  { path: "", redirectTo: "/login", pathMatch: "full" },
  { path: "login", component: LoginComponent },
  { path: "dashboard", component: DashboardComponent },
  { path: "user-management", component: UserManagementComponent, canActivate: [AdminGuard] },

  {
    path: "movements",
    loadComponent: () =>
      import("./components/movement-management.component").then(
        (m) => m.MovementManagementComponent,
      ),
  },
  {
    path: "movements/pret",
    loadComponent: () =>
      import("./components/pret-management/pret-management.component").then(
        (m) => m.PretManagementComponent,
      ),
  },
  {
    path: "equipment",
    loadComponent: () =>
      import("./components/equipment-management.component").then(
        (m) => m.EquipmentManagementComponent,
      ),
  },
  {
    path: "codification",
    loadComponent: () =>
      import("./components/codification-management.component").then(
        (m) => m.CodificationManagementComponent,
      ),
  },
  {
    path: "unauthorized",
    loadComponent: () =>
      import("./components/unauthorized/unauthorized.component").then(
        (m) => m.UnauthorizedComponent,
      ),
  },
  { path: "**", redirectTo: "/login" },
];
