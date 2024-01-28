<template>
  <q-page padding>
    <q-tabs v-model="tab" align="left" class="text-primary" dense>
      <q-tab name="reservations" label="Réservations" />
      <q-tab name="userInfo" label="Informations Utilisateur" />
      <q-tab name="wallet" label="Portefeuille" />
    </q-tabs>

    <q-tab-panels v-model="tab" animated>
      <q-tab-panel name="reservations">
        <q-card flat bordered class="q-pa-md">
          <div class="text-h6 q-mb-md">Réservations actuelles</div>

          <div class="row">
            <div class="col-12 col-md-6">
              <q-card-section class="q-pt-none">
                <div v-if="room" class="text-subtitle1">
                  Chambre sélectionnée : {{ room.Type }}
                </div>
                <div v-if="room && room.PricePerNight" class="text-subtitle2">
                  Prix par nuit : {{ room.PricePerNight.Amount }} EUR
                </div>
                <div v-if="room && room.Infos" class="text-subtitle2">
                  Informations :
                  <ul>
                    <li v-for="(info, index) in room.Infos" :key="index">
                      {{ info }}
                    </li>
                  </ul>
                </div>
              </q-card-section>
            </div>

            <div class="col-12 col-md-6">
              <q-card-section>
                <q-date
                  v-model="dateRange"
                  range
                  label="Sélectionnez la plage de dates"
                  class="q-mt-md full-width"
                />
                <q-input
                  v-model="numberOfRooms"
                  type="number"
                  min="1"
                  step="1"
                  label="Nombre de chambres"
                  class="q-mt-md full-width"
                />
                <div class="text-subtitle2 q-mt-md">
                  Nombre de nuits : <span>{{ numberOfNights }}</span>
                </div>
                <div class="text-subtitle2">
                  Montant total de la réservation :
                  <span>{{ totalAmount }} EUR</span>
                  <br />
                  Votre portefeuille :
                  <span
                    >{{
                      walletBalance.Balance ? walletBalance.Balance.Amount : 0
                    }}
                    EUR
                  </span>
                </div>
                <q-btn
                  label="Valider"
                  icon="check_circle"
                  type="button"
                  color="primary"
                  class="q-mt-md"
                  :disabled="totalAmount === 0"
                  @click="validateReservation"
                />
              </q-card-section>
            </div>
          </div>
        </q-card>
      </q-tab-panel>

      <q-tab-panel name="userInfo">
        <q-card flat bordered class="q-pa-md">
          <q-card-section>
            <div class="text-h6">
              Modifier les informations de l'utilisateur
            </div>
            <q-form @submit="updateUserInfo">
              <q-input v-model="user.FullName" label="Nom complet" />
              <q-input v-model="user.PhoneNumber.Number" label="Telephone" />
              <q-input v-model="user.Email.Value" label="Email" />
              <q-input v-model="user.PasswordHash" label="Mot de passe" />
              <q-btn label="Mettre à jour" type="submit" color="primary" />
              <q-btn
                label="Supprimer mon compte"
                type="button"
                color="negative"
                @click="deleteUser"
              />
            </q-form>
          </q-card-section>
        </q-card>
      </q-tab-panel>

      <q-tab-panel name="wallet">
        <q-card flat bordered class="q-pa-md">
          <q-card-section>
            <div class="text-h6">
              <q-icon name="account_balance_wallet" class="q-mr-sm" />
              Gerer son portefeuille
            </div>
            <div class="text-subtitle2">
              Entrez le montant que vous souhaitez ajouter à votre portefeuille
              :
            </div>
            <q-input
              v-model="amount"
              type="number"
              placeholder="Montant"
              class="q-mt-md"
            />
            <q-select
              v-model="currency"
              :options="currencies"
              label="Devise"
              class="q-mt-md"
            />
            <q-btn
              label="Recharger"
              color="primary"
              class="q-mt-md"
              @click="rechargeWallet"
            />
            <div class="text-subtitle2 q-mt-md">
              Solde du portefeuille :
              {{ walletBalance.Balance ? walletBalance.Balance.Amount : 0 }} EUR
            </div>
          </q-card-section>
        </q-card>
      </q-tab-panel>
    </q-tab-panels>
  </q-page>
</template>

<script setup lang="ts">
import { onMounted, ref, computed } from "vue";
import {
  getUserInfo,
  updateCustomer,
  deleteCustomer,
  creditWallet as apiRechargeWallet,
  getWallet as apiGetWalletBalance,
  createWallet as apiCreateWallet,
} from "../../api/services/api";
import { Customer, Currency, Room, Reservation } from "../../api/models/index";
import {
  getReservation,
  createReservation,
  updateReservationStatus,
  deleteReservation,
} from "../../api/services/reservationApi";

// Réactifs et états
const user = ref<Customer>({
  Id: "",
  FullName: "",
  Email: { Value: "" },
  PasswordHash: "",
  PhoneNumber: { Number: "" },
});
const reservationStatus = Object.freeze({
  PENDING: 1,
  CONFIRMED: 2,
  CANCELLED: 3,
});
const room = ref<Room | null>({
  Id: undefined,
  Type: undefined,
  PricePerNight: { Amount: undefined, Currency: undefined },
  Infos: undefined,
});
const tab = ref("reservations");
const amount = ref(0);
const currency = ref("EUR");
const walletBalance = ref({ Balance: { Amount: 0 } });
const dateRange = ref({ from: "", to: "" });
const numberOfRooms = ref(1);
const currencies = ref(Object.values(Currency));

// Calculs
const numberOfNights = computed(() => {
  if (dateRange.value && dateRange.value.from && dateRange.value.to) {
    const date1 = new Date(dateRange.value.from);
    const date2 = new Date(dateRange.value.to);
    const diffTime = Math.abs(date2.getTime() - date1.getTime());
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  }
  return 0;
});
const totalAmount = computed(() => {
  if (room.value?.PricePerNight?.Amount) {
    return (
      room.value.PricePerNight.Amount *
      numberOfNights.value *
      numberOfRooms.value
    );
  }
  return 0;
});

// Fonctions
const transformUserInfo = (userInfo: any): Customer => ({
  Id: userInfo.Id,
  FullName: userInfo.FullName,
  Email: { Value: userInfo.Email },
  PasswordHash: "",
  PhoneNumber: { Number: userInfo.PhoneNumber.Number },
});

const validateReservation = async () => {
  if (!user.value.Id || !room.value) {
    console.error("User ID or room is undefined");
    return;
  }

  const reservation = {
    newReservation: {
      Customer: {
        Id: user.value.Id,
        FullName: user.value.FullName,
        Email: { Value: user.value.Email?.Value },
        PhoneNumber: {
          Number: user.value.PhoneNumber?.Number,
        },
      },
      Room: {
        Id: room.value.Id,
        Type: room.value.Type,
        PricePerNight: room.value.PricePerNight?.Amount,
        Infos: room.value.Infos,
      },
      CheckInDate: dateRange.value.from,
      CheckOutDate: dateRange.value.to,
      NumberOfNights: numberOfNights.value,
      Status: reservationStatus.PENDING,
    },
  };

  console.log("reservation", reservation);
  try {
    await createReservation(reservation);
    console.log("Réservation créée avec succès");
  } catch (error: any) {
    console.error("Échec de la création de la réservation", error);
    if (error.response && error.response.data) {
      console.error("Response status:", error.response.status);
      console.error("Response data:", error.response.data);
    }
  }
};

const getWalletBalance = async () => {
  if (!user.value.Id) {
    console.error("User ID is undefined");
    return;
  }
  console.log("Getting wallet balance", user.value.Id);
  try {
    walletBalance.value = await apiGetWalletBalance(user.value.Id);
  } catch (error: any) {
    console.error("Failed to get wallet balance", error);
    if (error.response && error.response.status === 404) {
      console.log("Wallet not found, creating a new one", user.value.Id);
      await apiCreateWallet(user.value.Id);
      walletBalance.value = await apiGetWalletBalance(user.value.Id);
    }
  }
};
const rechargeWallet = async () => {
  if (!user.value.Id) {
    console.error("User ID is undefined");
    return;
  }
  console.log("Recharging wallet", user.value.Id, amount.value, currency.value);
  await apiRechargeWallet(user.value.Id, amount.value, currency.value);
  await getWalletBalance();
};
const updateUserInfo = async () => {
  if (!user.value.Id) {
    console.error("User ID is undefined");
    return;
  }
  console.log("update Dzata", user.value.Id, user.value);
  await updateCustomer(user.value.Id, user.value);
};
const deleteUser = async () => {
  if (!user.value.Id) {
    console.error("User ID is undefined");
    return;
  }
  console.log("Deleting user", user.value.Id);
  await deleteCustomer(user.value.Id);
};

// Gestion du cycle de vie
onMounted(async () => {
  const response = await getUserInfo();
  user.value = transformUserInfo(response);

  const roomString = localStorage.getItem("room");

  if (roomString) {
    try {
      room.value = JSON.parse(roomString);
      console.log("room.value:", room.value);
    } catch (error) {
      console.error("Failed to parse room data from localStorage", error);
    }
  } else {
    console.log('No data for "room"');
  }

  if (user.value.Id) {
    await getWalletBalance();
    if (walletBalance.value.Balance.Amount === 0) {
      await apiCreateWallet(user.value.Id);
      await getWalletBalance();
    }
  } else {
    console.error("User ID is undefined");
  }
});
</script>
<style>
.q-tab-panels {
  margin-top: 20px;
}

.q-card-section .q-input,
.q-card-section .q-date {
  max-width: 300px;
}
</style>
