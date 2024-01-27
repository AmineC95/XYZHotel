<template>
  <div class="login-container items-center q-px-xl">
    <q-card class="absolute-center q-px-xl">
      <q-tabs v-model="tab" active-color="primary" indicator-color="secondary">
        <q-tab name="login" label="Se connecter" />
        <q-tab name="register" label="Créer un compte" />
      </q-tabs>
      <q-card-section>
        <q-form
          @submit="loginCustomer"
          class="q-gutter-md"
          v-if="tab === 'login'"
        >
          <q-input
            label="Email"
            type="email"
            id="email"
            v-model="credentials.Email"
            required
          />
          <q-input
            label="Password"
            type="password"
            id="password"
            v-model="credentials.Password"
            required
          />
          <q-btn type="submit">Login</q-btn>
        </q-form>
        <q-form
          @submit="registerCustomer"
          class="q-gutter-md"
          v-if="tab === 'register'"
        >
          <q-input
            label="Nom complet"
            type="text"
            id="fullName"
            v-model="newCustomer.fullName"
            required
          />
          <q-input
            label="Email"
            type="email"
            id="email"
            v-model="newCustomer.email.value"
            required
          />
          <q-input
            label="Numéro de téléphone"
            type="tel"
            id="phoneNumber"
            v-model="newCustomer.phoneNumber.number"
            required
          />
          <q-input
            label="Mot de passe"
            type="password"
            id="password"
            v-model="newCustomer.password"
            required
          />
          <q-btn type="submit">Créer un compte</q-btn>
        </q-form>
      </q-card-section>
    </q-card>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from "vue";
import { useRouter } from "vue-router";
import { LoginRequest, Customer } from "../../api/models/index";
import { login, createCustomer } from "../../api/services/api";

const credentials = reactive<LoginRequest>({
  Email: "",
  Password: "",
});

const newCustomer = reactive({
  fullName: "",
  email: { value: "" },
  phoneNumber: { number: "" },
  password: "",
});

const tab = ref("login");
const router = useRouter();

const registerCustomer = async () => {
  console.log(
    newCustomer.fullName,
    newCustomer.email.value,
    newCustomer.phoneNumber.number,
    newCustomer.password,
  );
  try {
    const customerToCreate: Customer = {
      FullName: newCustomer.fullName,
      Email: { value: newCustomer.email.value },
      PhoneNumber: { number: newCustomer.phoneNumber.number },
      PasswordHash: newCustomer.password,
    };
    const response = await createCustomer(customerToCreate);
    console.log(response);
  } catch (error) {
    console.error(error);
  }
};

const loginCustomer = async () => {
  console.log(credentials.Email, credentials.Password);
  try {
    const response = await login(credentials);
    console.log(response);
    localStorage.setItem("token", response.Token);
    window.dispatchEvent(new Event("storage"));
    router.push("/dashboard");
  } catch (error) {
    console.error(error);
  }
};
</script>
