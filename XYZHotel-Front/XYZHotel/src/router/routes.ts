import { RouteRecordRaw } from "vue-router";

const routes: RouteRecordRaw[] = [
  {
    path: "/",
    component: () => import("layouts/MainLayout.vue"),
    children: [
      { path: "", component: () => import("pages/IndexPage.vue") },
      {
        name: "Login",
        path: "login",
        component: () => import("pages/Login/LoginCustomer.vue"),
      },
      {
        name: "Dashboard",
        path: "dashboard",
        component: () => import("pages/UserDashboard/CustomerDashboard.vue"),
      },
      {
        name: "Booking",
        path: "booking",
        component: () => import("pages/ReservationPage/ReservationPage.vue"),
      },
    ],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: "/:catchAll(.*)*",
    component: () => import("pages/ErrorNotFound.vue"),
  },
];

export default routes;
