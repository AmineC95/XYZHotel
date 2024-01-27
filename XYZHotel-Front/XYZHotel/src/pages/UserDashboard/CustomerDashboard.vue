<template>
  <q-page padding>
    <q-card>
      <q-card-section>
        <div class="text-h6">Réservations actuelles</div>
      </q-card-section>
    </q-card>
    <q-card>
      <q-card-section>
        <div class="text-h6">Modifier les informations de l'utilisateur</div>
        <q-form @submit="updateUserInfo">
          <q-input v-model="user.fullname" label="Nom complet" />
          <q-input v-model="user.email" label="Email" />
          <q-input v-model="user.password" label="Mot de passe" type="password" />
          <q-btn label="Mettre à jour" type="submit" color="primary" />
        </q-form>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup>
import { onMounted, ref } from 'vue';
import { getUserInfo } from "../../api/services/api";

const user = ref({
  fullname: '',
  email: '',
  phoneNumber: '',
  password: ''
});

const updateUserInfo = () => {

};

onMounted(async () => {
  const response = await getUserInfo();
  if (response.status === 200) {
    user.value.fullname = response.data.Fullname;
    user.value.email = response.data.Email;
  }
  console.log(response);
});
</script>
