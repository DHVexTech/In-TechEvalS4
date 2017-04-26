<template>
    <div>
        <div class="page-header">
            <h1 v-if="mode == 'create'">Créer un professeur</h1>
            <h1 v-else>Editer un professeur</h1>
        </div>

        <span v-if="messageError != null"><alert color="red" :message="messageError"></alert></span>


        <form @submit="onSubmit($event)">
            <div class="alert alert-danger" v-if="errors.length > 0">
                <b>Les champs suivants semblent invalides : </b>

                <ul>
                    <li v-for="e of errors">{{e}}</li>
                </ul>
            </div>

            <div class="form-group">
                <label class="required">Nom</label>
                <input type="text" v-model="item.lastName" class="form-control" required>
            </div>

            <div class="form-group">
                <label class="required">Prénom</label>
                <input type="text" v-model="item.firstName" class="form-control" required>
            </div>

            <input type="radio" id="0" value="0" v-model="item.isHere">
            <label for="0">Present</label>
            <br>
            <input type="radio" id="1" value="1" v-model="item.isHere">
            <label for="1">Retard</label>
            <br>
            <input type="radio" id="2" value="2" v-model="item.isHere">
            <label for="2">Absent</label>
            <br>

            <button type="submit" class="btn btn-primary">Sauvegarder</button>
        </form>
    </div>
</template>

<script>
    import { mapActions } from 'vuex'
    import TeacherApiService from '../../services/TeacherApiService'
    import Alert from './../MyAlert.vue'

    export default {
        components: {
            Alert
        },
        data () {
            return {
                item: {},
                mode: null,
                id: null,
                errors: [],
                messageError: null
            }
        },

        async mounted() {
            this.mode = this.$route.params.mode;
            this.id = this.$route.params.id;

            if(this.mode == 'edit') {
                try {
                    this.item = await this.executeAsyncRequest(() => TeacherApiService.getTeacherAsync(this.id));
                }
                catch(error) {
                    this.$router.replace('/teachers');
                }
            }
        },

        methods: {
            ...mapActions(['executeAsyncRequest']),

            async onSubmit(e) {
                e.preventDefault();

                var errors = [];

                if(!this.item.lastName) errors.push("Nom")
                if(!this.item.firstName) errors.push("Prénom")

                this.errors = errors;

                if(errors.length == 0) {
                    try {
                        if(this.mode == 'create') {
                            await this.executeAsyncRequest(() => TeacherApiService.createTeacherAsync(this.item));
                        }
                        else {
                            await this.executeAsyncRequest(() => TeacherApiService.updateTeacherAsync(this.item));
                        }

                        this.$router.replace('/teachers');
                    }
                    catch(error) {
                        this.messageError = error.responseText; 
                    }
                }
            }
        }
    }
</script>

<style lang="less">

</style>