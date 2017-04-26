<template>
    <div>
        <div class="page-header">
            <h1>Gestion des professeurs</h1>
        </div>

        <div class="panel panel-default">
            <div class="panel-body text-right">
                <router-link class="btn btn-primary" :to="`teachers/create`"><i class="glyphicon glyphicon-plus"></i> Ajouter un professeur</router-link>
            </div>
        </div>

        <span v-if='alertMessage == "Professeur supprimé."'><alert color="green" :message="alertMessage"></alert></span>

        <table class="table table-striped table-hover table-bordered">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Nom</th>
                    <th>Prénom</th>
                    <th>Options</th>
                    <th>Present</th>
                </tr>
            </thead>

            <tbody>
                <tr v-if="teacherList.length == 0">
                    <td colspan="4" class="text-center">Il n'y a actuellement aucun professeur.</td>
                </tr>

                <tr v-for="i of teacherList">
                    <td>{{ i.teacherId }}</td>
                    <td>{{ i.lastName }}</td>
                    <td>{{ i.firstName }}</td>
                    <td>
                        <router-link :to="`teachers/edit/${i.teacherId}`"><i class="glyphicon glyphicon-pencil"></i></router-link>
                        <router-link :to="`teachers/assign/${i.teacherId}`"><i class="glyphicon glyphicon-pushpin"></i></router-link>
                        <a href="#" @click="deleteTeacher(i.teacherId)"><i class="glyphicon glyphicon-remove"></i></a>
                    </td>
                    <td>{{i.isHere}}</td>
                </tr>
            </tbody>
        </table>
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
        data() {
            return {
                teacherList: [],
                alertMessage: null
            }
        },

        async mounted() {
            await this.refreshList();
        },

        methods: {
            ...mapActions(['executeAsyncRequestOrDefault', 'executeAsyncRequest']),

            async refreshList() {
                this.teacherList = await this.executeAsyncRequestOrDefault(() => TeacherApiService.getTeacherListAsync());
                console.log(this.teacherList);
                for(var i = 0; i < this.teacherList.length; i++)
                {
                    if(this.teacherList[i].isHere == 0)
                    {
                        this.teacherList[i].isHere = "Present";
                    }
                    else if (this.teacherList[i].isHere == 1)
                    {
                        this.teacherList[i].isHere = "Retard";
                    }
                    else
                    {
                        this.teacherList[i].isHere = "Absent";
                    }
                }
            },

            async deleteTeacher(teacherId) {
                try {
                    await this.executeAsyncRequest(() => TeacherApiService.deleteTeacherAsync(teacherId));
                    await this.refreshList();
                    this.alertMessage = "Professeur supprimé."
                }
                catch(error) {

                }
            }
        }
    }
</script>

<style lang="less">

</style>