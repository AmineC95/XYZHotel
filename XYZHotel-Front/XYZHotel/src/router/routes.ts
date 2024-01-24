import { RouteRecordRaw } from "vue-router";

const routes: RouteRecordRaw[] = [
  {
    path: "/",
    component: () => import("layouts/MainLayout.vue"),
    children: [
      { path: "", component: () => import("pages/IndexPage.vue") },
      {
        path: "login",
        component: () => import("pages/Login/LoginCustomer.vue"),
      },
      {
        path: "dashboard",
        component: () => import("pages/UserDashboard/CustomerDashboard.vue"),
      },
      {
        path: "reservation",
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
